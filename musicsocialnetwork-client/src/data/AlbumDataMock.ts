import { AlbumData } from '../models/Album';
import { tracksData } from './TrackDataMock';

export let albumData: Array<AlbumData> = [
  {
    id: 1,
    name: 'Seventh Son of a Seventh Son',
    yearReleased: 1988,
    artistId: 1,
    coverArtUrl: 'https://www.theaudiodb.com/images/media/album/thumb/seventh-son-of-a-seventh-son-4ddfeae2c5409.jpg',
    description: 'Seventh Son of a Seventh Son is the seventh studio album by British heavy metal band Iron Maiden, released in 1988 by EMI in Europe and its sister label Capitol Records in the U.S. (it was re-released by Sanctuary/Columbia Records in the U.S. in 2002).It is the first Iron Maiden release to feature keyboards and, along with The Number of the Beast and, later, Fear of the Dark and The Final Frontier, debuted at No. 1 in the UK Albums Chart. It also marks the first appearance of many progressive rock elements, particularly seen in the length and odd-time signatures of the title track "Seventh Son of a Seventh Son," and by the fact that it is a concept album.',
    format: 'Album',
    genre: 'Heavy Metal',
    style: 'Metal',
    ratingData: { averageRating: 7.94, ratingCount: 14103 },
    tracks: []
  },
  {
    id: 2,
    name: 'Somewhere in Time',
    yearReleased: 1986,
    artistId: 1,
    coverArtUrl: 'https://www.theaudiodb.com/images/media/album/thumb/somewhere-in-time-4ddfeae2d7cec.jpg',
    description: 'Somewhere in Time is the sixth studio album by British heavy metal band Iron Maiden, released on 29 September 1986 on EMI in Europe and its sister label Capitol Records in the U.S. (it was re-released by Sanctuary/Columbia Records in the U.S. in 1998). The studio follow-up to the hugely successful Powerslave/Live After Death pair, it was the first Iron Maiden album to feature guitar synthesizers. Since its release, it has been certified platinum by the RIAA, having sold over 1,000,000 copies in the US alone. Somewhere On Tour was the release\'s supporting tour.',
    format: 'Album',
    genre: 'Heavy Metal',
    style: 'Metal',
    ratingData: { averageRating: 7.88, ratingCount: 12647 },
    tracks: tracksData
  }
];