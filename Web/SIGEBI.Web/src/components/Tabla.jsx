export default function Tabla({ columnas, datos, acciones }) {
  return (
    <div style={{ overflowX: 'auto' }}>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead>
          <tr style={{ background: '#1F3864', color: 'white' }}>
            {columnas.map(c => <th key={c} style={{ padding: '10px 12px', textAlign: 'left' }}>{c}</th>)}
            {acciones && <th style={{ padding: '10px 12px' }}>Acciones</th>}
          </tr>
        </thead>
        <tbody>
          {datos.length === 0 ? (
            <tr><td colSpan={columnas.length + 1} style={{ padding: '20px', textAlign: 'center', color: '#888' }}>Sin registros</td></tr>
          ) : datos.map((fila, i) => (
            <tr key={i} style={{ background: i % 2 === 0 ? '#f9f9f9' : 'white', borderBottom: '1px solid #ddd' }}>
              {columnas.map(c => <td key={c} style={{ padding: '8px 12px' }}>{fila[c] ?? '-'}</td>)}
              {acciones && <td style={{ padding: '8px 12px' }}>{acciones(fila)}</td>}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
