export default function Mensaje({ texto, tipo }) {
  if (!texto) return null;
  const color = tipo === 'error' ? '#c00' : '#375623';
  return (
    <div style={{ padding: '10px 16px', margin: '10px 0', background: tipo === 'error' ? '#fdecea' : '#e9f7ef',
      border: '1px solid ' + color, borderRadius: '6px', color }}>
      {texto}
    </div>
  );
}
