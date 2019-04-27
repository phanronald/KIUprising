
import * as React from "react";

import { ICompressJpgProps, ICompressJpgState } from '../../../model/compressjpg/icompressjpg';

import { BaseSixtyFourImage } from '../global-components/base-sixty-four-image/base-sixty-four-image.component';

import './compressjpg.component.scss';

export class CompressJpgComponent extends React.Component<ICompressJpgProps, ICompressJpgState> {
	constructor(props: ICompressJpgProps) {
		super(props);
		this.state = {
			base64Images: []
		};
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {
	}
	render() {

		const { base64Images } = this.state;

		return (
			<>
				<div>
					<div>
						<span>Compressing JPG</span>
					</div>
					<div>
						<input id="imgFile" type="file" name="files" multiple accept="image/jpeg" />
					</div>
				</div>
			</>
		);

	}
}