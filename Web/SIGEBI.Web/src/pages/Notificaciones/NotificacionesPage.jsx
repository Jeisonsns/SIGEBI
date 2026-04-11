import { useEffect, useState } from 'react';
import notificacionService from '../../services/notificacionService';
import usuarioService from '../../services/usuarioService';
import Mensaje from '../../components/Mensaje';

export default function NotificacionesPage() {
  const [notificaciones, setNotificaciones] = useState([]);
  const [usuarios, setUsuarios] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [form, setForm] = useState({ usuarioId: '', tipo: 3, asunto: '', mensaje: '' });

  const cargar = async () => {
    try {
      const [n, u] = await Promise.all([notificacionService.getAll(), usuarioService.getAll()]);
      setNotificaciones(n.data); setUsuarios(u.data);
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try { await notificacionService.enviar({ ...form, tipo: parseInt(form.tipo) }); setMsg({ texto: 'Notificación enviada.', tipo: 'ok' }); setForm({ usuarioId: '', tipo: 3, asunto: '', mensaje: '' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleAutomatico = async (fn, texto) => {
    try { await fn(); setMsg({ texto, tipo: 'ok' }); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '8px 16px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '8px' });

  return (
    <div style={{ padding: '24px', maxWidth: '1100px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Gestión de Notificaciones</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />
      <div style={{ marginBottom: '16px' }}>
        <button style={btn('#C55A11')} onClick={() => handleAutomatico(notificacionService.notificarVencimientosProximos, 'Notificaciones de vencimientos próximos enviadas.')}>Notificar Vencimientos Próximos</button>
        <button style={btn('#7B2D8B')} onClick={() => handleAutomatico(notificacionService.notificarPrestamosVencidos, 'Notificaciones de préstamos vencidos enviadas.')}>Notificar Préstamos Vencidos</button>
      </div>
      <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '12px', marginBottom: '24px', background: '#f5f5f5', padding: '16px', borderRadius: '8px' }}>
        <select style={inp} required value={form.usuarioId} onChange={e => setForm({...form, usuarioId: e.target.value})}>
          <option value="">Seleccionar usuario *</option>
          {usuarios.map(u => <option key={u.id} value={u.id}>{u.nombre}</option>)}
        </select>
        <select style={inp} value={form.tipo} onChange={e => setForm({...form, tipo: e.target.value})}>
          <option value={0}>VencimientoProximo</option>
          <option value={1}>PrestamoVencido</option>
          <option value={2}>PenalizacionAplicada</option>
          <option value={3}>Confirmacion</option>
        </select>
        <input style={inp} placeholder="Asunto *" required value={form.asunto} onChange={e => setForm({...form, asunto: e.target.value})} />
        <textarea style={{...inp, gridColumn: 'span 2'}} placeholder="Mensaje *" required rows={2} value={form.mensaje} onChange={e => setForm({...form, mensaje: e.target.value})} />
        <button type="submit" style={{ ...btn('#1F3864'), alignSelf: 'end' }}>Enviar Notificación</button>
      </form>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead><tr style={{ background: '#1F3864', color: 'white' }}>
          {['Usuario','Tipo','Asunto','Fecha','Estado'].map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
        </tr></thead>
        <tbody>
          {notificaciones.length === 0 ? <tr><td colSpan={5} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          : notificaciones.map((n, i) => {
            const u = usuarios.find(x => x.id === n.usuarioId);
            return (
              <tr key={n.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
                <td style={{ padding: '8px 12px' }}>{u?.nombre ?? n.usuarioId}</td>
                <td style={{ padding: '8px 12px' }}>{n.tipo}</td>
                <td style={{ padding: '8px 12px' }}>{n.asunto}</td>
                <td style={{ padding: '8px 12px' }}>{new Date(n.fechaEnvio).toLocaleDateString()}</td>
                <td style={{ padding: '8px 12px' }}>{n.estado}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
