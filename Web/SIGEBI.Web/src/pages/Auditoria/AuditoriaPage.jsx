import { useEffect, useState } from 'react';
import auditoriaService from '../../services/auditoriaService';
import Mensaje from '../../components/Mensaje';

export default function AuditoriaPage() {
  const [registros, setRegistros] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [filtro, setFiltro] = useState({ desde: '', hasta: '', usuario: '' });

  const cargar = async () => {
    try { const res = await auditoriaService.getAll(); setRegistros(res.data); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleFiltrarFecha = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try { const res = await auditoriaService.getByFecha(filtro.desde, filtro.hasta); setRegistros(res.data); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleFiltrarUsuario = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try { const res = await auditoriaService.getByUsuario(filtro.usuario); setRegistros(res.data); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '8px 16px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '8px' });

  return (
    <div style={{ padding: '24px', maxWidth: '1200px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Auditoría del Sistema</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />
      <div style={{ display: 'flex', gap: '16px', marginBottom: '24px', flexWrap: 'wrap' }}>
        <form onSubmit={handleFiltrarFecha} style={{ display: 'flex', gap: '8px', alignItems: 'flex-end', background: '#f5f5f5', padding: '12px', borderRadius: '8px' }}>
          <div><label style={{ fontSize: '12px', display: 'block', marginBottom: '4px' }}>Desde</label><input style={{...inp, width:'160px'}} type="datetime-local" value={filtro.desde} onChange={e => setFiltro({...filtro, desde: e.target.value})} /></div>
          <div><label style={{ fontSize: '12px', display: 'block', marginBottom: '4px' }}>Hasta</label><input style={{...inp, width:'160px'}} type="datetime-local" value={filtro.hasta} onChange={e => setFiltro({...filtro, hasta: e.target.value})} /></div>
          <button type="submit" style={btn('#2E75B6')}>Filtrar por Fecha</button>
        </form>
        <form onSubmit={handleFiltrarUsuario} style={{ display: 'flex', gap: '8px', alignItems: 'flex-end', background: '#f5f5f5', padding: '12px', borderRadius: '8px' }}>
          <div><label style={{ fontSize: '12px', display: 'block', marginBottom: '4px' }}>Usuario</label><input style={{...inp, width:'180px'}} placeholder="Nombre de usuario" value={filtro.usuario} onChange={e => setFiltro({...filtro, usuario: e.target.value})} /></div>
          <button type="submit" style={btn('#2E75B6')}>Filtrar por Usuario</button>
        </form>
        <button style={{...btn('#888'), alignSelf:'flex-end'}} onClick={cargar}>Ver Todos</button>
      </div>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
        <thead><tr style={{ background: '#1F3864', color: 'white' }}>
          {['Operación','Usuario','Entidad','Fecha','Resultado','Detalles'].map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
        </tr></thead>
        <tbody>
          {registros.length === 0 ? <tr><td colSpan={6} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          : registros.map((r, i) => (
            <tr key={r.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
              <td style={{ padding: '8px 12px' }}>{r.operacion}</td>
              <td style={{ padding: '8px 12px' }}>{r.usuario}</td>
              <td style={{ padding: '8px 12px' }}>{r.entidad}</td>
              <td style={{ padding: '8px 12px' }}>{new Date(r.fecha).toLocaleString()}</td>
              <td style={{ padding: '8px 12px', color: r.resultado === 'Exitoso' ? '#375623' : '#c00' }}>{r.resultado}</td>
              <td style={{ padding: '8px 12px' }}>{r.detalles}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
