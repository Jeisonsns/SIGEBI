import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Navbar from './components/Navbar';
import RecursosPage from './pages/Recursos/RecursosPage';
import UsuariosPage from './pages/Usuarios/UsuariosPage';
import PrestamosPage from './pages/Prestamos/PrestamosPage';
import DevolucionesPage from './pages/Devoluciones/DevolucionesPage';
import PenalizacionesPage from './pages/Penalizaciones/PenalizacionesPage';
import NotificacionesPage from './pages/Notificaciones/NotificacionesPage';
import AuditoriaPage from './pages/Auditoria/AuditoriaPage';

export default function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<Navigate to="/recursos" />} />
        <Route path="/recursos" element={<RecursosPage />} />
        <Route path="/usuarios" element={<UsuariosPage />} />
        <Route path="/prestamos" element={<PrestamosPage />} />
        <Route path="/devoluciones" element={<DevolucionesPage />} />
        <Route path="/penalizaciones" element={<PenalizacionesPage />} />
        <Route path="/notificaciones" element={<NotificacionesPage />} />
        <Route path="/auditoria" element={<AuditoriaPage />} />
      </Routes>
    </BrowserRouter>
  );
}
