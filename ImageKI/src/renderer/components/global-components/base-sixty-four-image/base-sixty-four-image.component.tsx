// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from "react";

import { IBaseSixtyFourImageProps, IBaseSixtyFourImageState } from './../../../../model/client/basesixtyfourimage/ibasesixtyfourimage';

import { SaveImage } from './../save-image-prompt/save-image.component';

import './base-sixty-four-image.component.scss';

export class BaseSixtyFourImage extends React.Component<IBaseSixtyFourImageProps, IBaseSixtyFourImageState> {

	constructor(props: IBaseSixtyFourImageProps) {
		super(props);

		this.state = {
			base64Href: '',
			imageMimeType: ''
		};
	}

	public componentDidMount() {
		this.setState({
			base64Href: 'data:' + this.props.imageMimeType + ';base64,' + this.props.base64Image,
			imageMimeType: this.props.imageMimeType
		});
	}

	public componentDidUpdate(prevProps: IBaseSixtyFourImageProps, prevState: IBaseSixtyFourImageState) {
		if (this.props.base64Image !== prevProps.base64Image) {
			this.setState({
				base64Href: 'data:' + this.props.imageMimeType + ';base64,' + this.props.base64Image,
				imageMimeType: this.props.imageMimeType
			});
		}
	}

	public componentWillUnmount() {

	}

	render() {

		const { base64Href, imageMimeType } = this.state;

		return (
			<div className='compressed-image-container'>
				<div className="compressed-image">
					<img src={base64Href} />
				</div>
				<div>
					<SaveImage base64Href={base64Href} imageMimeType={imageMimeType} />
				</div>
			</div>
		);

	}
}