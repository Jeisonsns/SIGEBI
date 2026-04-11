import api from './api';

const auditoriaService = {
  getAll: () => api.get('/Auditoria'),
  getById: (id) => api.get('/Auditoria/' + id),
  getByFecha: (desde, hasta) => api.get('/Auditoria/fecha?desde=' + desde + '&hasta=' + hasta),
  getByUsuario: (usuario) => api.get('/Auditoria/usuario/' + usuario),
  save: (dto) => api.post('/Auditoria', dto),
};

export default auditoriaService;
