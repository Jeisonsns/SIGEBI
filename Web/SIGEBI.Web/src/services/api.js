import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  headers: { 'Content-Type': 'application/json' },
  timeout: 15000,
});

api.interceptors.response.use(
  response => response,
  error => {
    if (!error.response) {
      return Promise.reject({ message: 'No se pudo conectar con el servidor.' });
    }
    const msg = error.response.data?.message || 'Error inesperado del servidor.';
    return Promise.reject({ message: msg, status: error.response.status });
  }
);

export default api;
