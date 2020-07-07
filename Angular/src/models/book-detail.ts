import {Tag} from './tag';
import {Author} from './author';

export class BookDetail {
  public id: number;
  public name: string;
  public description: string;
  public tags: Tag[];
  public authors: Author[];
  public coverUrl: string;
  public assessment: number;
}
