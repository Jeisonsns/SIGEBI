import api from './api';

const penalizacionService = {
  getAll: () => api.get('/Penalizaciones'),
  getById: (id) => api.get('/Penalizaciones/' + id),
  getActivasByUsuario: (usuarioId) => api.get('/Penalizaciones/activas/' + usuarioId),
  save: (dto) => api.post('/Penalizaciones', dto),
  update: (dto) => api.put('/Penalizaciones', dto),
  resolver: (id) => api.patch('/Penalizaciones/' + id + '/resolver'),
  remove: (id) => api.delete('/Penalizaciones/' + id),
};

export default penalizacionService;
