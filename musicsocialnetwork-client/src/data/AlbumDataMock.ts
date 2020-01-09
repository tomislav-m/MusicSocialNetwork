import { AlbumData } from '../models/Album';
import { tracksData } from './TrackDataMock';

export let albumData: Array<AlbumData> = [
  {
    Id: 1,
    Name: 'Seventh Son of a Seventh Son',
    YearReleased: 1988,
    ArtistId: 1,
    CoverArtUrl: 'https://www.theaudiodb.com/images/media/album/thumb/seventh-son-of-a-seventh-son-4ddfeae2c5409.jpg',
    Description: 'Seventh Son of a Seventh Son is the seventh studio album by British heavy metal band Iron Maiden, released in 1988 by EMI in Europe and its sister label Capitol Records in the U.S. (it was re-released by Sanctuary/Columbia Records in the U.S. in 2002).It is the first Iron Maiden release to feature keyboards and, along with The Number of the Beast and, later, Fear of the Dark and The Final Frontier, debuted at No. 1 in the UK Albums Chart. It also marks the first appearance of many progressive rock elements, particularly seen in the length and odd-time signatures of the title track "Seventh Son of a Seventh Son," and by the fact that it is a concept album.',
    Format: 'Album',
    Genre: 'Heavy Metal',
    Style: 'Metal',
    RatingData: { AverageRating: 7.94, RatingCount: 14103 },
    Tracks: []
  },
  {
    Id: 2,
    Name: 'Somewhere in Time',
    YearReleased: 1986,
    ArtistId: 1,
    CoverArtUrl: 'https://www.theaudiodb.com/images/media/album/thumb/somewhere-in-time-4ddfeae2d7cec.jpg',
    Description: 'Somewhere in Time is the sixth studio album by British heavy metal band Iron Maiden, released on 29 September 1986 on EMI in Europe and its sister label Capitol Records in the U.S. (it was re-released by Sanctuary/Columbia Records in the U.S. in 1998). The studio follow-up to the hugely successful Powerslave/Live After Death pair, it was the first Iron Maiden album to feature guitar synthesizers. Since its release, it has been certified platinum by the RIAA, having sold over 1,000,000 copies in the US alone. Somewhere On Tour was the release\'s supporting tour.',
    Format: 'Album',
    Genre: 'Heavy Metal',
    Style: 'Metal',
    RatingData: { AverageRating: 7.88, RatingCount: 12647 },
    Tracks: tracksData
  }
];