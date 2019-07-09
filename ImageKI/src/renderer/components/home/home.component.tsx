import * as React from "react";

import './home.component.css';

export class HomeComponent extends React.Component<any, any> {

	constructor(props: any) {
		super(props);
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {
	}

	private displayChromeVersion = ():string => {
		return navigator.appVersion.match(/.*Chrome\/([0-9\.]+)/)[1];
	}

	render() {

		return (
			<>
				<div>
					<h4>Welcome to React, Electron and Typescript with Router Upgraded v2</h4>
					<h3>{this.displayChromeVersion()}</h3>
					<div className="gif-container">
						<img className="freezeframe" alt="Azur Lane Gif" 
							src="./../images/gifs/azur-lane.gif"></img>
					</div>
				</div>
			</>
		);

	}
}