import { Link } from 'react-router-dom';

export default function Navbar() {
  return (
    <nav style={{ background: '#1F3864', padding: '12px 24px', display: 'flex', gap: '20px', alignItems: 'center' }}>
      <span style={{ color: 'white', fontWeight: 'bold', fontSize: '18px', marginRight: '20px' }}>SIGEBI</span>
      {['Recursos','Usuarios','Prestamos','Devoluciones','Penalizaciones','Notificaciones','Auditoria'].map(m => (
        <Link key={m} to={'/' + m.toLowerCase()}
          style={{ color: '#ccc', textDecoration: 'none', fontSize: '14px' }}>
          {m}
        </Link>
      ))}
    </nav>
  );
}
