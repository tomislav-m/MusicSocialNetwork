import React from 'react';
import { Track } from '../../models/Track';
import { List, Label, Segment } from 'semantic-ui-react';

interface TrackProps {
  tracks: Array<Track>;
}

export default class Tracks extends React.Component<TrackProps> {
  public render() {
    const tracks = this.props.tracks;
    let overallTime = 0;

    return (
      <Segment>
        <Label ribbon>Track listing</Label>
        <List divided>
          {
            tracks.map((track, index) => {
              overallTime += track.Duration;
              return (
                <List.Item key={index}>
                  <strong>{index + 1}. {track.Title}</strong> ({this.secondsToMinute(track.Duration)})
                </List.Item>
              );
            })
          }
          <List.Item></List.Item>
          <List.Item>
            <strong>Overall: </strong>
            {this.secondsToMinute(overallTime)}
          </List.Item>
        </List>
      </Segment>
    );
  }

  private secondsToMinute(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remaining = (seconds % 60).toFixed(0);
    return minutes + ':' + remaining;
  }
}