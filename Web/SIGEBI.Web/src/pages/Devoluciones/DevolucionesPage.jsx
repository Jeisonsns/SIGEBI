import { useEffect, useState } from 'react';
import devolucionService from '../../services/devolucionService';
import prestamoService from '../../services/prestamoService';
import Mensaje from '../../components/Mensaje';

export default function DevolucionesPage() {
  const [devoluciones, setDevoluciones] = useState([]);
  const [prestamosActivos, setPrestamosActivos] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [prestamoId, setPrestamoId] = useState('');

  const cargar = async () => {
    try {
      const [d, p] = await Promise.all([devolucionService.getAll(), prestamoService.getAll()]);
      setDevoluciones(d.data);
      setPrestamosActivos(p.data.filter(x => x.estado === 'Activo'));
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleProcesar = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try { await devolucionService.procesar(prestamoId); setMsg({ texto: 'Devolución procesada.', tipo: 'ok' }); setPrestamoId(''); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '8px 16px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' });

  return (
    <div style={{ padding: '24px', maxWidth: '1100px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Gestión de Devoluciones</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />
      <form onSubmit={handleProcesar} style={{ display: 'grid', gridTemplateColumns: '1fr auto', gap: '12px', marginBottom: '24px', background: '#f5f5f5', padding: '16px', borderRadius: '8px' }}>
        <select style={inp} required value={prestamoId} onChange={e => setPrestamoId(e.target.value)}>
          <option value="">Seleccionar préstamo activo *</option>
          {prestamosActivos.map(p => <option key={p.id} value={p.id}>Préstamo {p.id.slice(0,8)}... — vence {new Date(p.fechaLimite).toLocaleDateString()}</option>)}
        </select>
        <button type="submit" style={{ ...btn('#1F3864'), alignSelf: 'end' }}>Procesar Devolución</button>
      </form>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead><tr style={{ background: '#1F3864', color: 'white' }}>
          {['Préstamo ID','Fecha Devolución','¿Tardía?','Días Retraso'].map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
        </tr></thead>
        <tbody>
          {devoluciones.length === 0 ? <tr><td colSpan={4} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          : devoluciones.map((d, i) => (
            <tr key={d.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
              <td style={{ padding: '8px 12px' }}>{d.prestamoId?.slice(0,12)}...</td>
              <td style={{ padding: '8px 12px' }}>{new Date(d.fechaDevolucion).toLocaleDateString()}</td>
              <td style={{ padding: '8px 12px', color: d.esTardia ? '#c00' : '#375623' }}>{d.esTardia ? 'Sí' : 'No'}</td>
              <td style={{ padding: '8px 12px' }}>{d.diasRetraso ?? 0}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
