import * as React from "react";

import './home.component.scss';

export class HomeComponent extends React.Component<any, any> {
	constructor(props: any) {
		super(props);
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {
	}

	render() {

		return (
			<>
				<div>
					<h4>Welcome to React, Electron and Typescript with Router</h4>
				</div>
			</>
		);

	}
}