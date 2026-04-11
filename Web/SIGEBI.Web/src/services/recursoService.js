import api from './api';

const recursoService = {
  getAll: () => api.get('/Recursos'),
  getById: (id) => api.get('/Recursos/' + id),
  getByEstado: (estado) => api.get('/Recursos/estado/' + estado),
  save: (dto) => api.post('/Recursos', dto),
  update: (dto) => api.put('/Recursos', dto),
  cambiarEstado: (id, estado) => api.patch('/Recursos/' + id + '/estado', JSON.stringify(estado)),
  remove: (id) => api.delete('/Recursos/' + id),
};

export default recursoService;
