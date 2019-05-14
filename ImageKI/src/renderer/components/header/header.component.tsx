import * as React from "react";

import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import PhoneIcon from '@material-ui/icons/Phone';
import FavoriteIcon from '@material-ui/icons/Favorite';
import PersonPinIcon from '@material-ui/icons/PersonPin';
import HelpIcon from '@material-ui/icons/Help';
import ShoppingBasket from '@material-ui/icons/ShoppingBasket';
import ThumbDown from '@material-ui/icons/ThumbDown';
import ThumbUp from '@material-ui/icons/ThumbUp';

import { IHeaderProps, IHeaderState } from './../../../model/header/iheader';

import { CompressPngComponent } from './../compresspng/compresspng.component';
import { CompressJpgComponent } from './../compressjpg/compressjpg.component';
import { UnarchiveComponent } from './../unarchive/unarchive.component';
import { ConvertImageComponent } from './../convertimage/convertimage.component';

import './header.component.scss';

export class HeaderComponent extends React.Component<IHeaderProps, IHeaderState> {


	constructor(props: IHeaderProps) {
		super(props);
		this.state = {
			tabIndex: 0
		};
	}

	public componentDidMount() {

	}

	public componentWillUnmount() {

	}

	private onTabChange = (event: any, value: number): void => {

		this.setState({
			tabIndex: value
		});

	}

	render() {

		const { tabIndex } = this.state;

		return (
			<>
				<div className='app-bar'>
					<AppBar position="static" color="default">
						<Tabs value={tabIndex} onChange={this.onTabChange}
							variant="scrollable" scrollButtons="on" indicatorColor="primary"
							textColor="primary">

							<Tab label="Compress PNG" icon={<PhoneIcon />} />
							<Tab label="Compress JPG" icon={<FavoriteIcon />} />
							<Tab label="Convert JPG to PNG" icon={<PersonPinIcon />} />
							<Tab label="UnArchive" icon={<HelpIcon />} />
							<Tab label="Item Five" icon={<ShoppingBasket />} />
							<Tab label="Item Six" icon={<ThumbDown />} />
							<Tab label="Item Seven" icon={<ThumbUp />} />
						</Tabs>
					</AppBar>
					{
						tabIndex === 0 &&
						<CompressPngComponent />
					}
					{
						tabIndex === 1 &&
						<CompressJpgComponent />
					}
					{
						tabIndex === 2 &&
						<ConvertImageComponent />
					}
					{
						tabIndex === 3 &&
						<UnarchiveComponent />
					}
					{
						tabIndex === 4 &&
						<div>Item Five</div>
					}
					{
						tabIndex === 5 &&
						<div>Item Six</div>
					}
					{
						tabIndex === 6 &&
						<div>Item Seven</div>
					}
				</div>
			</>
		);

	}
}