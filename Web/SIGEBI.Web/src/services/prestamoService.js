import api from './api';

const prestamoService = {
  getAll: () => api.get('/Prestamos'),
  getById: (id) => api.get('/Prestamos/' + id),
  getActivosByUsuario: (usuarioId) => api.get('/Prestamos/activos/' + usuarioId),
  getVencidos: () => api.get('/Prestamos/vencidos'),
  save: (dto) => api.post('/Prestamos', dto),
  update: (dto) => api.put('/Prestamos', dto),
  renovar: (id) => api.patch('/Prestamos/' + id + '/renovar'),
  remove: (id) => api.delete('/Prestamos/' + id),
};

export default prestamoService;
