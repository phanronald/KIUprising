import * as React from "react";

import { ICompressPngProps, ICompressPngState } from './../../../model/client/compresspng/icompresspng';

import { BaseSixtyFourImage } from './../global-components/base-sixty-four-image/base-sixty-four-image.component';

import './compresspng.component.css';

export class CompressPngComponent extends React.Component<ICompressPngProps, ICompressPngState> {
	constructor(props: ICompressPngProps) {
		super(props);
		this.state = {
			base64Images: []
		};
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {
	}

	private async sendImagePngToCompress() {
		let imageFiles = document.getElementById("imgFile") as HTMLInputElement;
		let formData = new FormData();

		if (imageFiles.files.length > 0) {

			for (let i = 0; i < imageFiles.files.length; i++) {
				const imageFile = imageFiles.files[i];
				if (imageFile.type === 'image/png') {
					formData.append('pngImgFiles', imageFile);
				}
			}
		}

		let response = await fetch('https://localhost:44375/api/CompressImg/CompressPng', {
			method: "POST", // *GET, POST, PUT, DELETE, etc.
			mode: "cors", // no-cors, cors, *same-origin
			cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
			credentials: "same-origin", // include, *same-origin, omit
			redirect: "follow", // manual, *follow, error
			referrer: "no-referrer", // no-referrer, *client
			body: formData, // body data type must match "Content-Type" header
		});

		const imageBaseREsponse = await response.text();
		const imageBaseSixtyFourStrings = JSON.parse(imageBaseREsponse);

		this.setState({
			base64Images: imageBaseSixtyFourStrings
		});
	}

	render() {

		const { base64Images } = this.state;

		return (
			<>
				<div>
					<div>
						<span>Compressing PNG</span>
					</div>
					<div>
						<input id="imgFile" type="file" name="files" multiple accept="image/png"
							onChange={evt => this.sendImagePngToCompress()} />
					</div>
				</div>
				<div>
					{
						base64Images.map((base64Image: string, index: number) => {

							return (
								<BaseSixtyFourImage key={index} base64Image={base64Image}
									imageMimeType={'image/png'} />
							)

						})
					}
				</div>
			</>
		);

	}
}