
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

		switch (this.props.imageMimeType) {
			case 'image/png': {
				anchorTag.download = 'image.png';
				break;
			}
		}

		anchorTag.click();
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