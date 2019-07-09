
import * as React from "react";
import Button from '@material-ui/core/Button';

import { ISaveImageProps, ISaveImageState } from './../../../../model/client/saveimage/isaveimage';

import './save-image.component.css';

export class SaveImage extends React.Component<ISaveImageProps, ISaveImageState> {

	constructor(props: ISaveImageProps) {
		super(props);

		this.state = {
			base64Href: '',
			imageMimeType: ''
		};
	}

	public componentDidMount() {
		this.setState({
			base64Href: this.props.base64Href,
			imageMimeType: this.props.imageMimeType
		});
	}

	public componentDidUpdate(prevProps: ISaveImageProps, prevState: ISaveImageState) {
		if (this.props.base64Href !== prevProps.base64Href) {
			this.setState({
				base64Href: this.props.base64Href,
				imageMimeType: this.props.imageMimeType
			});
		}
	}

	public componentWillUnmount() {

	}

	private promptSaveImage = (): void => {
		let anchorTag = document.createElement('a');
		anchorTag.href = this.state.base64Href;

		let foundImage: boolean = false;
		switch (this.state.imageMimeType) {
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

		return (
			<>
				<Button className="save-img-btn" onClick={evt => this.promptSaveImage()}>Save Image</Button>
			</>
		);

	}
}