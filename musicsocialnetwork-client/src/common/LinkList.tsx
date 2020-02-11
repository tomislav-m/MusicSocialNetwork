import React from 'react';
import { ArtistDataSimple } from '../models/Artist';
import { Link } from 'react-router-dom';

interface LinkListProps {
  artists: Array<ArtistDataSimple> | undefined;
}

export default class LinkList extends React.Component<LinkListProps> {
  render() {
    const { artists } = this.props;

    return (
      <span>
        {
          artists && artists.map((artist, index) =>
            <span>
              {
                index > 0 ? ', ' : ''
              }
              {
                <Link to={`/Artist/${artist.id}`}>{artist.name}</Link>
              }
            </span>
          )
        }
      </span>
    );
  }
}