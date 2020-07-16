import {Tag} from './tag';
import {Author} from './author';
import {AuthUser} from '../app/auth/models/auth.user';

export class BookDetail {
  public id: number;
  public name: string;
  public description: string;
  public tags: Tag[];
  public authors: Author[];
  public fanUsers: AuthUser[];
  public coverUrl: string;
  public assessment: number;
}
