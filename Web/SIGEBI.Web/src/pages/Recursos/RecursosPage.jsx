import { useEffect, useState } from 'react';
import recursoService from '../../services/recursoService';
import Mensaje from '../../components/Mensaje';

export default function RecursosPage() {
  const [recursos, setRecursos] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [form, setForm] = useState({ titulo: '', autor: '', isbn: '', categoria: '', editorial: '', anio: '', numEjemplares: '' });
  const [editId, setEditId] = useState(null);

  const cargar = async () => {
    try {
      const res = await recursoService.getAll();
      setRecursos(res.data);
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMsg({ texto: '', tipo: '' });
    try {
      if (editId) {
        await recursoService.update({ ...form, id: editId, estado: 0 });
        setMsg({ texto: 'Recurso actualizado.', tipo: 'ok' });
      } else {
        await recursoService.save(form);
        setMsg({ texto: 'Recurso registrado.', tipo: 'ok' });
      }
      setForm({ titulo: '', autor: '', isbn: '', categoria: '', editorial: '', anio: '', numEjemplares: '' });
      setEditId(null);
      cargar();
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleEditar = (r) => {
    setEditId(r.id);
    setForm({ titulo: r.titulo, autor: r.autor, isbn: r.isbn, categoria: r.categoria, editorial: r.editorial, anio: r.anio, numEjemplares: r.numEjemplares });
  };

  const handleEliminar = async (id) => {
    if (!window.confirm('¿Eliminar este recurso?')) return;
    try {
      await recursoService.remove(id);
      setMsg({ texto: 'Recurso eliminado.', tipo: 'ok' });
      cargar();
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '8px 16px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '6px' });

  return (
    <div style={{ padding: '24px', maxWidth: '1100px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Gestión de Recursos</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />

      <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '12px', marginBottom: '24px', background: '#f5f5f5', padding: '16px', borderRadius: '8px' }}>
        <input style={inp} placeholder="Título *" required value={form.titulo} onChange={e => setForm({...form, titulo: e.target.value})} />
        <input style={inp} placeholder="Autor *" required value={form.autor} onChange={e => setForm({...form, autor: e.target.value})} />
        <input style={inp} placeholder="ISBN" value={form.isbn} onChange={e => setForm({...form, isbn: e.target.value})} />
        <input style={inp} placeholder="Categoría" value={form.categoria} onChange={e => setForm({...form, categoria: e.target.value})} />
        <input style={inp} placeholder="Editorial" value={form.editorial} onChange={e => setForm({...form, editorial: e.target.value})} />
        <input style={inp} placeholder="Año" type="number" value={form.anio} onChange={e => setForm({...form, anio: e.target.value})} />
        <input style={inp} placeholder="Num. Ejemplares *" type="number" required value={form.numEjemplares} onChange={e => setForm({...form, numEjemplares: e.target.value})} />
        <div style={{ display: 'flex', alignItems: 'flex-end', gap: '8px' }}>
          <button type="submit" style={btn('#1F3864')}>{editId ? 'Actualizar' : 'Registrar'}</button>
          {editId && <button type="button" style={btn('#888')} onClick={() => { setEditId(null); setForm({ titulo:'',autor:'',isbn:'',categoria:'',editorial:'',anio:'',numEjemplares:'' }); }}>Cancelar</button>}
        </div>
      </form>

      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead>
          <tr style={{ background: '#1F3864', color: 'white' }}>
            {['Título','Autor','ISBN','Categoría','Ejemplares','Estado','Acciones'].map(c => (
              <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {recursos.length === 0 ? (
            <tr><td colSpan={7} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          ) : recursos.map((r, i) => (
            <tr key={r.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
              <td style={{ padding: '8px 12px' }}>{r.titulo}</td>
              <td style={{ padding: '8px 12px' }}>{r.autor}</td>
              <td style={{ padding: '8px 12px' }}>{r.isbn}</td>
              <td style={{ padding: '8px 12px' }}>{r.categoria}</td>
              <td style={{ padding: '8px 12px' }}>{r.numEjemplares}</td>
              <td style={{ padding: '8px 12px' }}>{r.estado}</td>
              <td style={{ padding: '8px 12px' }}>
                <button style={btn('#2E75B6')} onClick={() => handleEditar(r)}>Editar</button>
                <button style={btn('#c00')} onClick={() => handleEliminar(r.id)}>Eliminar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
