import { useEffect, useState } from 'react';
import prestamoService from '../../services/prestamoService';
import usuarioService from '../../services/usuarioService';
import recursoService from '../../services/recursoService';
import Mensaje from '../../components/Mensaje';

export default function PrestamosPage() {
  const [prestamos, setPrestamos] = useState([]);
  const [usuarios, setUsuarios] = useState([]);
  const [recursos, setRecursos] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [form, setForm] = useState({ usuarioId: '', recursoId: '' });

  const cargar = async () => {
    try {
      const [p, u, r] = await Promise.all([prestamoService.getAll(), usuarioService.getAll(), recursoService.getAll()]);
      setPrestamos(p.data); setUsuarios(u.data); setRecursos(r.data);
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try { await prestamoService.save(form); setMsg({ texto: 'Préstamo autorizado.', tipo: 'ok' }); setForm({ usuarioId: '', recursoId: '' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleRenovar = async (id) => {
    try { await prestamoService.renovar(id); setMsg({ texto: 'Préstamo renovado.', tipo: 'ok' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleEliminar = async (id) => {
    if (!window.confirm('¿Eliminar este préstamo?')) return;
    try { await prestamoService.remove(id); setMsg({ texto: 'Préstamo eliminado.', tipo: 'ok' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '6px 12px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '4px' });

  return (
    <div style={{ padding: '24px', maxWidth: '1100px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Gestión de Préstamos</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />
      <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr auto', gap: '12px', marginBottom: '24px', background: '#f5f5f5', padding: '16px', borderRadius: '8px' }}>
        <select style={inp} required value={form.usuarioId} onChange={e => setForm({...form, usuarioId: e.target.value})}>
          <option value="">Seleccionar usuario *</option>
          {usuarios.map(u => <option key={u.id} value={u.id}>{u.nombre} ({u.codigo})</option>)}
        </select>
        <select style={inp} required value={form.recursoId} onChange={e => setForm({...form, recursoId: e.target.value})}>
          <option value="">Seleccionar recurso *</option>
          {recursos.filter(r => r.estado === 'Disponible').map(r => <option key={r.id} value={r.id}>{r.titulo}</option>)}
        </select>
        <button type="submit" style={{ ...btn('#1F3864'), alignSelf: 'end', whiteSpace: 'nowrap' }}>Solicitar Préstamo</button>
      </form>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead><tr style={{ background: '#1F3864', color: 'white' }}>
          {['Usuario','Recurso','Fecha Inicio','Fecha Límite','Estado','Acciones'].map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
        </tr></thead>
        <tbody>
          {prestamos.length === 0 ? <tr><td colSpan={6} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          : prestamos.map((p, i) => {
            const u = usuarios.find(x => x.id === p.usuarioId);
            const r = recursos.find(x => x.id === p.recursoId);
            return (
              <tr key={p.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
                <td style={{ padding: '8px 12px' }}>{u?.nombre ?? p.usuarioId}</td>
                <td style={{ padding: '8px 12px' }}>{r?.titulo ?? p.recursoId}</td>
                <td style={{ padding: '8px 12px' }}>{new Date(p.fechaInicio).toLocaleDateString()}</td>
                <td style={{ padding: '8px 12px' }}>{new Date(p.fechaLimite).toLocaleDateString()}</td>
                <td style={{ padding: '8px 12px' }}>{p.estado}</td>
                <td style={{ padding: '8px 12px' }}>
                  {p.estado === 'Activo' && <button style={btn('#2E75B6')} onClick={() => handleRenovar(p.id)}>Renovar</button>}
                  <button style={btn('#c00')} onClick={() => handleEliminar(p.id)}>Eliminar</button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
