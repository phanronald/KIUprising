// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from "react";

import { IBaseSixtyFourImageProps, IBaseSixtyFourImageState } from './../../../../model/basesixtyfourimage/ibasesixtyfourimage';

import { SaveImage } from './../save-image-prompt/save-image.component';

import './base-sixty-four-image.component.scss';

export class BaseSixtyFourImage extends React.Component<IBaseSixtyFourImageProps, IBaseSixtyFourImageState> {

	private base64Href: string = '';

	constructor(props: IBaseSixtyFourImageProps) {
		super(props);

		this.base64Href = 'data:' + this.props.imageMimeType + ';base64,' + this.props.base64Image;
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {

	}

	render() {

		const { imageMimeType } = this.props;

		return (
			<div className='compressed-image-container'>
				<div className="compressed-image">
					<img src={this.base64Href} />
				</div>
				<div>
					<SaveImage base64Href={this.base64Href} imageMimeType={imageMimeType} />
				</div>
			</div>
		);

	}
}