import * as React from "react";

import { IUnarchiveProps, IUnarchiveState } from './../../../model/client/unarchive/iunarchive';

import './unarchive.component.scss';

export class UnarchiveComponent extends React.Component<IUnarchiveProps, IUnarchiveState> {
	constructor(props: IUnarchiveProps) {
		super(props);
		this.state = {
			archiveDirectory: {}
		};
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {
	}

	private async sendZipToUnArchive() {
		let archiveFiles = document.getElementById("zipFile") as HTMLInputElement;
		let formData = new FormData();

		if (archiveFiles.files.length > 0) {

			for (let i = 0; i < archiveFiles.files.length; i++) {
				const archiveFile = archiveFiles.files[i];
				if (archiveFile.type === 'application/x-zip-compressed') {
					formData.append('zipFiles', archiveFile);
				}
				else {
					const fileName = archiveFile.name;
					if (fileName.indexOf('.7z') || fileName.indexOf('.rar') ||
						fileName.indexOf('.tar') || fileName.indexOf('.gz')) {

						formData.append('zipFiles', archiveFile);
					}
				}
			}
		}

		let response = await fetch('https://localhost:44375/api/ArchiveFile/ArchiveSevenZip', {
			method: "POST", // *GET, POST, PUT, DELETE, etc.
			mode: "cors", // no-cors, cors, *same-origin
			cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
			credentials: "same-origin", // include, *same-origin, omit
			redirect: "follow", // manual, *follow, error
			referrer: "no-referrer", // no-referrer, *client
			body: formData, // body data type must match "Content-Type" header
		});

		const zipBaseRepsonse = await response.text();
		const zipResponseParsed = JSON.parse(zipBaseRepsonse);

		this.setState({
			archiveDirectory: zipResponseParsed
		});
	}

	render() {

		return (
			<>
				<div>
					<div>
						<span>Unarchive files</span>
					</div>
					<div>
						<input id="zipFile" type="file" name="files" multiple
							accept=".zip,.rar,.7z,.tar,.gz"
							onChange={evt => this.sendZipToUnArchive()} />
					</div>
				</div>
				<div>

				</div>
			</>
		);

	}
}