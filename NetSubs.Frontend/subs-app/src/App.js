import React from 'react';
import Container from 'react-bootstrap/Container';
import SubscriptionTable from './components/SubscriptionTable';

function App() {
  return (
    <Container fluid style={{ paddingTop: '2rem' }}>
      <h1>Subscriptions:</h1>
      <SubscriptionTable />
    </Container>
  );
}

export default App;