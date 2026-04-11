import { useEffect, useState } from 'react';
import usuarioService from '../../services/usuarioService';
import Mensaje from '../../components/Mensaje';

export default function UsuariosPage() {
  const [usuarios, setUsuarios] = useState([]);
  const [msg, setMsg] = useState({ texto: '', tipo: '' });
  const [form, setForm] = useState({ codigo: '', nombre: '', email: '', tipo: 0 });
  const [editId, setEditId] = useState(null);

  const cargar = async () => {
    try { const res = await usuarioService.getAll(); setUsuarios(res.data); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  useEffect(() => { cargar(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault(); setMsg({ texto: '', tipo: '' });
    try {
      if (editId) { await usuarioService.update({ ...form, id: editId, estado: 0 }); setMsg({ texto: 'Usuario actualizado.', tipo: 'ok' }); }
      else { await usuarioService.save(form); setMsg({ texto: 'Usuario registrado.', tipo: 'ok' }); }
      setForm({ codigo: '', nombre: '', email: '', tipo: 0 }); setEditId(null); cargar();
    } catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const handleEliminar = async (id) => {
    if (!window.confirm('¿Eliminar este usuario?')) return;
    try { await usuarioService.remove(id); setMsg({ texto: 'Usuario eliminado.', tipo: 'ok' }); cargar(); }
    catch (e) { setMsg({ texto: e.message, tipo: 'error' }); }
  };

  const tiposUsuario = ['Estudiante', 'Docente', 'PersonalBibliotecario', 'Administrador'];
  const inp = { padding: '8px', border: '1px solid #ccc', borderRadius: '4px', width: '100%', boxSizing: 'border-box' };
  const btn = (color) => ({ padding: '8px 16px', background: color, color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '6px' });

  return (
    <div style={{ padding: '24px', maxWidth: '1100px', margin: '0 auto' }}>
      <h2 style={{ color: '#1F3864' }}>Gestión de Usuarios</h2>
      <Mensaje texto={msg.texto} tipo={msg.tipo} />
      <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr 1fr', gap: '12px', marginBottom: '24px', background: '#f5f5f5', padding: '16px', borderRadius: '8px' }}>
        <input style={inp} placeholder="Código *" required value={form.codigo} onChange={e => setForm({...form, codigo: e.target.value})} />
        <input style={inp} placeholder="Nombre *" required value={form.nombre} onChange={e => setForm({...form, nombre: e.target.value})} />
        <input style={inp} placeholder="Email *" required type="email" value={form.email} onChange={e => setForm({...form, email: e.target.value})} />
        <select style={inp} value={form.tipo} onChange={e => setForm({...form, tipo: parseInt(e.target.value)})}>
          {tiposUsuario.map((t, i) => <option key={i} value={i}>{t}</option>)}
        </select>
        <div style={{ display: 'flex', alignItems: 'flex-end' }}>
          <button type="submit" style={btn('#1F3864')}>{editId ? 'Actualizar' : 'Registrar'}</button>
          {editId && <button type="button" style={btn('#888')} onClick={() => { setEditId(null); setForm({ codigo:'',nombre:'',email:'',tipo:0 }); }}>Cancelar</button>}
        </div>
      </form>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead><tr style={{ background: '#1F3864', color: 'white' }}>
          {['Código','Nombre','Email','Tipo','Estado','Acciones'].map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
        </tr></thead>
        <tbody>
          {usuarios.length === 0 ? <tr><td colSpan={6} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          : usuarios.map((u, i) => (
            <tr key={u.id} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
              <td style={{ padding: '8px 12px' }}>{u.codigo}</td>
              <td style={{ padding: '8px 12px' }}>{u.nombre}</td>
              <td style={{ padding: '8px 12px' }}>{u.email}</td>
              <td style={{ padding: '8px 12px' }}>{u.tipo}</td>
              <td style={{ padding: '8px 12px' }}>{u.estado}</td>
              <td style={{ padding: '8px 12px' }}>
                <button style={btn('#2E75B6')} onClick={() => { setEditId(u.id); setForm({ codigo: u.codigo, nombre: u.nombre, email: u.email, tipo: u.tipo === 'Estudiante' ? 0 : u.tipo === 'Docente' ? 1 : u.tipo === 'PersonalBibliotecario' ? 2 : 3 }); }}>Editar</button>
                <button style={btn('#c00')} onClick={() => handleEliminar(u.id)}>Eliminar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
