import api from './api';

const devolucionService = {
  getAll: () => api.get('/Devoluciones'),
  getById: (id) => api.get('/Devoluciones/' + id),
  procesar: (prestamoId) => api.post('/Devoluciones/procesar/' + prestamoId),
  update: (dto) => api.put('/Devoluciones', dto),
  remove: (id) => api.delete('/Devoluciones/' + id),
};

export default devolucionService;
