import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import { Layout } from './components/layout/layout';

import { HomeComponent } from './components/home/home.component';

export const routes = <Layout><Switch>
	<Route exact path='/' component={HomeComponent} />
</Switch></Layout>;
