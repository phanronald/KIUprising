
import * as React from 'react';

import { HeaderComponent } from './../header/header.component';

import './layout.css';

export interface LayoutProps {
	children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, any> {

	constructor(props: any) {
		super(props);
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {

	}

	public render() {
		return (
			<div className='main-container clearfix'>
				<HeaderComponent />
				<div className='app-theme content-container'>
					{this.props.children}
				</div>
			</div>
		);
	}
}
