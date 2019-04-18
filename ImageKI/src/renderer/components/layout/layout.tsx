
import * as React from 'react';

import './layout.scss';

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
				<div className='content-container'>
					{this.props.children}
				</div>
			</div>
		);
	}
}
