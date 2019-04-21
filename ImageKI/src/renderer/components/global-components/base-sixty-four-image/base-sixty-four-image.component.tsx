// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from "react";

import { IBaseSixtyFourImageProps, IBaseSixtyFourImageState } from './../../../../model/basesixtyfourimage/ibasesixtyfourimage';

import './base-sixty-four-image.component.scss';

export class BaseSixtyFourImage extends React.Component<IBaseSixtyFourImageProps, IBaseSixtyFourImageState> {

	constructor(props: IBaseSixtyFourImageProps) {
		super(props);
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {

	}

	render() {

		const { base64Image, imageMimeType } = this.props;

		return (
			<div className="compressed-image">
				<img src={'data:' + imageMimeType + ';base64,' + base64Image} />
			</div>
		);

	}
}