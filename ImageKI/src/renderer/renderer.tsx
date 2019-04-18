import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { MemoryRouter, Switch, Route } from 'react-router';
import * as RoutesModule from './routes';

let routes = RoutesModule.routes;

function renderApp() {
    ReactDOM.render(
        <MemoryRouter children={routes}></MemoryRouter>,
        document.getElementById('react-app')
    );
}

renderApp();
