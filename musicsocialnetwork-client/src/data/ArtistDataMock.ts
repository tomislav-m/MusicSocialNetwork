import { ExtendedArtistData } from '../models/Artist';
import { eventData } from './EventDataMock';

export let artistData: Array<ExtendedArtistData> = [
  {
    id: 1,
    name: 'Iron Maiden',
    photoUrl: 'https://www.theaudiodb.com/images/media/artist/thumb/rtpxtt1385762058.jpg',
    bio: 'Iron Maiden are an English heavy metal band from Leyton in east London, formed in 1975 by bassist and primary songwriter Steve Harris. Since their inception, the band\'s discography has grown to include a total of thirty-six albums: fifteen studio albums; ten live albums; four EPs; and seven compilations. Pioneers of the New Wave of British Heavy Metal, Iron Maiden achieved success during the early 1980s. After several line-up changes, the band went on to release a series of U.S. and UK platinum and gold albums, including 1982\'s The Number of the Beast, 1983\'s Piece of Mind, 1984\'s Powerslave, 1985\'s live release Live After Death, 1986\'s Somewhere in Time and 1988\'s Seventh Son of a Seventh Son. Since the return of lead vocalist Bruce Dickinson and guitarist Adrian Smith in 1999, the band have undergone a resurgence in popularity, with their latest studio offering, The Final Frontier, peaking at No. 1 in 28 different countries and receiving widespread critical acclaim. Considered one of the most successful heavy metal bands in history, Iron Maiden have reportedly sold over 85 million records worldwide with little radio or television support. The band won the Ivor Novello Award for international achievement in 2002, and were also inducted into the Hollywood RockWalk in Sunset Boulevard, Los Angeles, California during their United States tour in 2005. As of August 2011, the band have played almost 2000 live shows throughout their career. For the past 30 years, the band have been supported by their famous mascot, "Eddie", who has appeared on almost all of their album and single covers, as well as in their live shows.',
    country: 'Leyton, England',
    facebookUrl: 'www.facebook.com/ironmaiden',
    websiteUrl: 'www.ironmaiden.com',
    yearBorn: 0,
    yearFormed: 1975,
    genres: undefined,
    styles: undefined,
    events: eventData,
    albums: []
  },
  {
    id: 2,
    name: 'Dream Theater',
    photoUrl: 'https://www.theaudiodb.com/images/media/artist/thumb/tqsvpt1363622181.jpg',
    bio: '',
    country: 'Massachusetts, USA',
    facebookUrl: 'www.facebook.com/pages/Dream-Theater/7677942180',
    websiteUrl: 'www.dreamtheater.net',
    yearBorn: 0,
    yearFormed: 1986,
    genres: undefined,
    styles: undefined,
    events: eventData,
    albums: []
  }
];