
export const arrayToList = (array: Array<any>): string => {
  let list = '';
  array.forEach((x, index) => {
    if (index > 0) {
      list += ', ';
    }
    list += x;
  });

  return list;
};