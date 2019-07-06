import { DirectoryService } from './../../server/directory-service';
import { ProcessService } from './../../server/process-service';
import { IShredder } from './../../model/server/shredder/ishredder';

export class ShredderService {

    private settings: IShredder;
    private shredFlags: string[] = [];
    private fileShredPath = DirectoryService.GetCurrentDirectory() + '\\process\\shred\\shred.exe';

	constructor(options?: IShredder) {

        this.settings = {
            forceShred: true,
            iterations: 3,
            deleteShred: true,
            hideShred: true,
            debugMode: true
        };

        if(options) {
            Object.assign(this.settings, options);
        }

        this.shredFlags = this.BuildShredFlags();
    }
    
    public ShredFile = (files: string[], endCallback?: (message: string, files: string[]) => void,
            statusCallback?: (action: string, progress: number, file: string, 
                            activePath: string) => void): void => {

        if(this.settings.debugMode) {
            console.log("shredfile: Shredding initiated.");
        }
        
        if(files.length === 0) {
            console.log("shredfile: No file(s) specified to shred!");
            console.log(typeof files);
            console.dir(files);
        }

        let file = DirectoryService.GetBaseName(files[0]);
        let activeFilePath = DirectoryService.GetDirectoryName(files[0]);
        const options = Array.from(new Set(this.shredFlags.concat(files)));
        const shred = ProcessService.GetChildWithoutStream(this.fileShredPath, options);
        if(this.settings.debugMode) {
            console.log('shredfile: Configured shred command: ' + this.fileShredPath + 
                ' ' + options.join(' '));
        }

        shred.on('close', (code: number, signal: string) => {

            if(typeof endCallback == 'function') {
                if(code === 0) {
                    endCallback(null,files);
                }
                else {
                    endCallback('Shredding completed with issues. Exit Code: ' + code, files);
                }
            }

        });

        shred.stderr.on('data', (data) => {
            
            if(this.settings.debugMode) {
                console.log('shredfile: stderr: ' + data);
            }

            let matches;
            let progress = 0;
            let rename = '';

            data = data.toString().replace(/(\r\n|\n|\r)/gm,"");
            const validInfo = new RegExp("^" + this.fileShredPath);
            if(data.match(validInfo)) {

                if(matches = data.match(/(\/[^:]+)\: pass (\d+)\/(\d+)/)) {
                    activeFilePath = DirectoryService.GetDirectoryName(matches[1]);
                    file = DirectoryService.GetBaseName(matches[1]);
                    const numerator = parseInt(matches[2]);
                    const denominator = parseInt(matches[3]);
                    if(denominator !== 0) {
                        progress = numerator / denominator;
                    }

                    if(typeof statusCallback == 'function') {
                        return statusCallback('overwriting', progress, file, activeFilePath);
                    }
                }

                matches = data.match(/renamed to (\/.*)$/);
                if(Array.isArray(matches)) {
                    rename =  DirectoryService.GetBaseName(matches[1]).trim();
                    if(!rename.match(/^[0]+$/)) {
                        return;
                    }

                    if(file.length > 0) {
                        progress = rename.length / file.length;
                    }

                    if(typeof statusCallback == 'function') {
                        return statusCallback('renaming', progress, file, activeFilePath);
                    }
                }

            }

        });
    }

    private BuildShredFlags = ():Array<string> => {

        let flagsArray: Array<string> = ['-v'];
        if(this.settings.forceShred) {
            flagsArray.push('-f');
        }

        flagsArray.push('--iterations=' + this.settings.iterations);
        
        if(this.settings.deleteShred) {
            flagsArray.push('-u');
        }

        if(this.settings.hideShred) {
            flagsArray.push('-z');
        }

        return flagsArray;
    }
}
