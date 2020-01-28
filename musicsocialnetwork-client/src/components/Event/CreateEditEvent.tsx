import React from 'react';
import { ArtistDataSimple } from '../../models/Artist';
import { EventData, defaultEvent } from '../../models/Event';
import { Form, Input, Dropdown, Button } from 'semantic-ui-react';
import autobind from 'autobind-decorator';
import _ from 'lodash';
import { createEvent } from '../../actions/Events/EventActions';

interface CreateEditEventProps {
  headliner?: ArtistDataSimple;
  supporter?: ArtistDataSimple;
  oldEvent?: EventData;
  isEdit: boolean;

  onEventSave(): void;
}

interface CreateEditEventState {
  headliners: Array<ArtistDataSimple>;
  selectedHeadliners: Array<number>;
  supporters: Array<ArtistDataSimple>;
  selectedSupporters: Array<number>;
  event: EventData;
}

export default class CreateEditEvent extends React.Component<CreateEditEventProps, CreateEditEventState> {
  constructor(props: CreateEditEventProps) {
    super(props);

    const headliners = [];
    const selectedHeadliners = [];
    const event = props.oldEvent ? _.cloneDeep(props.oldEvent) : defaultEvent;
    if (props.headliner) {
      headliners.push(props.headliner);
      selectedHeadliners.push(props.headliner.id);

      event.headliners = selectedHeadliners;
    }

    this.state = {
      headliners,
      supporters: [],
      event,
      selectedHeadliners,
      selectedSupporters: []
    };
  }

  public render() {
    const event = this.state.event;
    const headlinersOptions = this.artistsToOptions(this.state.headliners);
    const supportersOptions = this.artistsToOptions(this.state.supporters);

    return (
      <Form>
        <Form.Field>
          <label>Date</label>
          <Input name="date" type="date" value={this.dateToString(event?.date)} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Venue</label>
          <Input name="venue" type="text" value={event?.venue} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Headliners</label>
          <Dropdown
            name="headliners"
            search
            multiple
            selection
            value={this.state.selectedHeadliners}
            options={headlinersOptions}
            onChange={this.handleChangeHeadliners}
          />
        </Form.Field>
        <Form.Field>
          <label>Supporters</label>
          <Dropdown
            name="supporters"
            search
            multiple
            selection
            options={supportersOptions}
            onChange={this.handleChangeSupporters}
          />
        </Form.Field>
        <Button onClick={() => createEvent(this.state.event)}>Save</Button>
      </Form>
    );
  }

  @autobind
  handleChange(event: any, { value }: any) {
    const name = event.target.name;
    const eventData = { ...this.state.event, [name]: value };
    this.setState({
      event: eventData
    });
  }

  @autobind
  handleChangeHeadliners(event: any, { value }: any) {
    const eventData = { ...this.state.event, ['headliners']: value };
    this.setState({ selectedHeadliners: value });
  }

  @autobind
  handleChangeSupporters(event: any, { value }: any) {
    this.setState({ supporters: value });
  }

  private dateToString(date: Date | undefined) {
    if (typeof (date) === 'string') {
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
}