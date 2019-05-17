import { InvalidIdInfo } from './InvalidIdInfo';
import { ValidIdInfo } from './ValidIdInfo';
export interface IdInfo {
    validIdInfos: ValidIdInfo[];
    invalidIdInfos: InvalidIdInfo[];
}
