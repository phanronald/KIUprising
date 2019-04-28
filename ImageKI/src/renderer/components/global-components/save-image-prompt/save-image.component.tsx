
import * as React from "react";
import Button from '@material-ui/core/Button';

import { ISaveImageProps, ISaveImageState } from './../../../../model/saveimage/isaveimage';

import './save-image.component.scss';

export class SaveImage extends React.Component<ISaveImageProps, ISaveImageState> {

	constructor(props: ISaveImageProps) {
		super(props);
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {

	}

	private promptSaveImage = (): void => {
		let anchorTag = document.createElement('a');
		anchorTag.href = this.props.base64Href;

		let foundImage: boolean = false;
		switch (this.props.imageMimeType) {
			case 'image/png': {
				foundImage = true;
				anchorTag.download = 'image.png';
				break;
			}
			case 'image/jpg':
			case 'image/jpeg': {
				foundImage = true;
				anchorTag.download = 'image.jpg';
				break;
			}
		}

		if (foundImage) {
			anchorTag.click();
		}
	}

	render() {

		const { base64Href } = this.props;

		return (
			<>
				<Button className="save-img-btn" onClick={evt => this.promptSaveImage()}>Save Image</Button>
			</>
		);

	}
}