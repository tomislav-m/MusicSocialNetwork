import { ArtistDataSimple } from '../models/Artist';

export const arrayToList = (array: Array<ArtistDataSimple>): string => {
  let list = '';
  array.map(x => x.Name).forEach((x, index) => {
    if (index > 0) {
      list += ', ';
    }
    list += x;
  });

  return list;
};