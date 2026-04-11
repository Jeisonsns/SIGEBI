import { useEffect, useState } from 'react';
import penalizacionService from '../../services/penalizacionService';
import usuarioService from '../../services/usuarioService';
import Mensaje from '../../components/Mensaje';

export default function PenalizacionesPage() {
  const [penalizaciones, setPenalizaciones] = useState([]);
  const [usuarios, setUsuarios] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [form, setForm] = useState({ usuarioId: '', causa: '', tipo: 0, fechaFin: '' });

  const cargar = async () => {
    try {
      const [p, u] = await Promise.all([penalizacionService.getAll(), usuarioService.getAll()]);
      setPenalizaciones(p.data); setUsuarios(u.data);
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try { await penalizacionService.save({ ...form, tipo: parseInt(form.tipo) }); setMsg({ texto: 'Penalización aplicada.', tipo: 'ok' }); setForm({ usuarioId: '', causa: '', tipo: 0, fechaFin: '' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleResolver = async (id) => {
    try { await penalizacionService.resolver(id); setMsg({ texto: 'Penalización resuelta.', tipo: 'ok' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '6px 12px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '4px' });

  return (
    <div style={{ padding: '24px', maxWidth: '1100px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Gestión de Penalizaciones</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />
      <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr 1fr', gap: '12px', marginBottom: '24px', background: '#f5f5f5', padding: '16px', borderRadius: '8px' }}>
        <select style={inp} required value={form.usuarioId} onChange={e => setForm({...form, usuarioId: e.target.value})}>
          <option value="">Seleccionar usuario *</option>
          {usuarios.map(u => <option key={u.id} value={u.id}>{u.nombre}</option>)}
        </select>
        <input style={inp} placeholder="Causa *" required value={form.causa} onChange={e => setForm({...form, causa: e.target.value})} />
        <select style={inp} value={form.tipo} onChange={e => setForm({...form, tipo: e.target.value})}>
          <option value={0}>SuspensionTemporal</option>
          <option value={1}>Multa</option>
          <option value={2}>Advertencia</option>
        </select>
        <input style={inp} type="datetime-local" required value={form.fechaFin} onChange={e => setForm({...form, fechaFin: e.target.value})} />
        <button type="submit" style={btn('#1F3864')}>Aplicar Penalización</button>
      </form>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead><tr style={{ background: '#1F3864', color: 'white' }}>
          {['Usuario','Causa','Tipo','Estado','Fecha Fin','Acciones'].map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
        </tr></thead>
        <tbody>
          {penalizaciones.length === 0 ? <tr><td colSpan={6} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          : penalizaciones.map((p, i) => {
            const u = usuarios.find(x => x.id === p.usuarioId);
            return (
              <tr key={p.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
                <td style={{ padding: '8px 12px' }}>{u?.nombre ?? p.usuarioId}</td>
                <td style={{ padding: '8px 12px' }}>{p.causa}</td>
                <td style={{ padding: '8px 12px' }}>{p.tipo}</td>
                <td style={{ padding: '8px 12px', color: p.estado === 'Activa' ? '#c00' : '#375623' }}>{p.estado}</td>
                <td style={{ padding: '8px 12px' }}>{new Date(p.fechaFin).toLocaleDateString()}</td>
                <td style={{ padding: '8px 12px' }}>
                  {p.estado === 'Activa' && <button style={btn('#375623')} onClick={() => handleResolver(p.id)}>Resolver</button>}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
