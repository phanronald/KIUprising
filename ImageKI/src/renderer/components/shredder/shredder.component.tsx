import * as React from "react";

import { DirectoryService } from './../../../server/directory-service';
import { ShredderService } from './../../../services/shredder/shredder-service';

import './shredder.component.scss';

export class ShredderComponent extends React.Component<any, any> {

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

	private async shredFile() {
		let htmlFiles = document.getElementById("shredFile") as HTMLInputElement;

		if (htmlFiles.files.length > 0) {

			this.shredderService.ShredFile([htmlFiles.files[0].path]);
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
			</>
		);

	}
}