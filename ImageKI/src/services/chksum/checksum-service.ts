import { DirectoryService } from './../../server/directory-service';

export class CheckSumService {

    private chkSumPath = DirectoryService.GetCurrentDirectory() + '\\process\\chksum\\cksum.exe';

	constructor() {
    }
}
