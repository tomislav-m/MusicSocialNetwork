import React from 'react';
import { ArtistDataSimple } from '../../models/Artist';
import { EventData, defaultEvent } from '../../models/Event';
import { Form, Input, Dropdown, Button } from 'semantic-ui-react';
import autobind from 'autobind-decorator';
import _ from 'lodash';

interface CreateEditEventProps {
  headliner?: ArtistDataSimple;
  supporter?: ArtistDataSimple;
  oldEvent?: EventData;
  isEdit: boolean;

  onEventSave(): void;
}

interface CreateEditEventState {
  headliners: Array<number>;
  supporters: Array<number>;
  event: EventData;
}

export default class CreateEditEvent extends React.Component<CreateEditEventProps, CreateEditEventState> {
  constructor(props: CreateEditEventProps) {
    super(props);

    const headliners = props.oldEvent?.headliners || [];
    const supporters = props.oldEvent?.supporters || [];
    const event = props.oldEvent ? _.cloneDeep(props.oldEvent) : defaultEvent;
    this.state = {
      headliners,
      supporters,
      event
    };
  }

  public render() {
    const event = this.state.event;

    return (
      <Form>
        <Form.Field>
          <label>Date</label>
          <Input name="Date" type="date" value={this.dateToString(event?.date)} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Venue</label>
          <Input name="VenueName" type="text" value={event?.venueName} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Headliners</label>
          <Dropdown
            name="Headliners"
            search
            multiple
            selection
            value={this.state.headliners}
            options={this.artistsToOptions(this.artists)}
            onChange={this.handleChangeHeadliners}
          />
        </Form.Field>
        <Form.Field>
          <label>Supporters</label>
          <Dropdown
            name="Supporters"
            search
            multiple
            selection
            value={this.state.supporters}
            options={this.artistsToOptions(this.artists)}
            onChange={this.handleChangeSupporters}
          />
        </Form.Field>
        <Button onClick={this.props.onEventSave}>Save</Button>
      </Form>
    );
  }

  @autobind
  handleChange(event: any, {value}: any) {
    const name = event.target.name;
    const eventData = {...this.state.event, [name]: value};
    this.setState({
      event: eventData
    });
  }

  @autobind
  handleChangeHeadliners(event: any, {value}: any) {
    const headliners = [...this.state.headliners];
    headliners.push(value);
    this.setState({ headliners: value });
  }

  @autobind
  handleChangeSupporters(event: any, {value}: any) {
    this.setState({ supporters: value });
  }

  private dateToString(date: Date | undefined) {
    if (typeof(date) === 'string') {
      return date;
    }

    if (date) {
      const year = date.getFullYear();
      let month = (date.getMonth() + 1).toString();
      if (month.length === 1) {
        month = '0' + month;
      }
      let day = date.getDate().toString();
      if (day.length === 1) {
        day = '0' + day;
      }

      return year + '-' + month + '-' + day;
    }
  }

  private artistsToDropdownValues(artists: Array<ArtistDataSimple> | undefined) {
    if (artists) {
      return artists.map(x => {
        return x.id;
      });
    }
  }

  private artistsToOptions(artists: Array<ArtistDataSimple> | undefined) {
    if (artists) {
      return artists.map(x => {
        return {
          key: x.id,
          value: x.id,
          text: x.name
        };
      });
    }
  }

  private artists: Array<ArtistDataSimple> = [
    { id: 1, name: 'Iron Maiden' },
    { id: 2, name: 'Dream Theater' }
  ];
}