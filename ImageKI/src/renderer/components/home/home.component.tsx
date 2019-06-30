import * as React from "react";

import { DirectoryService } from './../../../server/directory-service';
import { ShredderService } from './../../../services/shredder/shredder-service';

import './home.component.scss';

export class HomeComponent extends React.Component<any, any> {

	private shredderService: ShredderService;

	constructor(props: any) {
		super(props);
		this.shredderService = new ShredderService();
		//DirectoryService.IsFileExist('C:\\Users\\Ronald\\Documents\\visualstudiogithub\\test.txt');
		//DirectoryService.IsFileExist('./shredtemp/shredthis.txt');
		DirectoryService.IsFileExist(DirectoryService.GetCurrentDirectory() + '\\shredtemp');
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {
	}

	private displayChromeVersion = ():string => {
		return navigator.appVersion.match(/.*Chrome\/([0-9\.]+)/)[1];
	}

	private async shredFile() {
		let htmlFiles = document.getElementById("shredFile") as HTMLInputElement;

		if (htmlFiles.files.length > 0) {

			this.shredderService.ShredFile([htmlFiles.files[0].path]);
			//console.log(htmlFiles.files[0].path, "TEST");
		}
	}

	render() {

		return (
			<>
				<div>
					<div>
						<span>Shredder Section</span>
					</div>
					<div>
						<input id="shredFile" type="file" name="files"
							onChange={evt => this.shredFile()} />
					</div>
				</div>
				<div>
					<h4>Welcome to React, Electron and Typescript with Router Upgraded v2</h4>
					<h3>{this.displayChromeVersion()}</h3>
				</div>
			</>
		);

	}
}