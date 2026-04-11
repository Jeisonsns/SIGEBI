import api from './api';

const usuarioService = {
  getAll: () => api.get('/Usuarios'),
  getById: (id) => api.get('/Usuarios/' + id),
  getByCodigo: (codigo) => api.get('/Usuarios/codigo/' + codigo),
  verificarAcceso: (id) => api.get('/Usuarios/' + id + '/acceso'),
  save: (dto) => api.post('/Usuarios', dto),
  update: (dto) => api.put('/Usuarios', dto),
  cambiarEstado: (id, estado) => api.patch('/Usuarios/' + id + '/estado', JSON.stringify(estado)),
  remove: (id) => api.delete('/Usuarios/' + id),
};

export default usuarioService;
