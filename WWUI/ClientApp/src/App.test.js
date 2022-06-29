import React from 'react';
import ReactDOM from 'react-dom';
import { MemoryRouter,useLocation } from 'react-router-dom';
import App from './App';
import Login from "./components/page/Login"

it('renders without crashing', async () => {
  const div = document.createElement('div');
  const locationer = useLocation();
  console.log(locationer.pathname)
  if (locationer.pathname!=="/") {
    <MemoryRouter>
      <Login/>
    </MemoryRouter>
  } else {
    ReactDOM.render(
    <MemoryRouter>
      <App />
    </MemoryRouter>, div);
  }
  
  await new Promise(resolve => setTimeout(resolve, 1000));
});
