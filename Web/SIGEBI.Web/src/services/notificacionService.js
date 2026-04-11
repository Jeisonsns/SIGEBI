import api from './api';

const notificacionService = {
  getAll: () => api.get('/Notificaciones'),
  getById: (id) => api.get('/Notificaciones/' + id),
  getByUsuario: (usuarioId) => api.get('/Notificaciones/usuario/' + usuarioId),
  enviar: (dto) => api.post('/Notificaciones/enviar', dto),
  notificarVencimientosProximos: () => api.post('/Notificaciones/vencimientos-proximos'),
  notificarPrestamosVencidos: () => api.post('/Notificaciones/prestamos-vencidos'),
  update: (dto) => api.put('/Notificaciones', dto),
  remove: (id) => api.delete('/Notificaciones/' + id),
};

export default notificacionService;
