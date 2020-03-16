import React from "react";
import { Field } from "react-final-form";
import { Form as SemanticForm } from "semantic-ui-react";
import GenericWizard from "../../common/genericWizard";
import Validators from "../../../utils/formValidators";

const AddMarkerWizard = ({ onClose, onSubmit, initialValues }) => {
  return (
    <GenericWizard title={"Add location"} onClose={onClose} onSubmit={onSubmit} initialValues={initialValues} showSteps={false}>
      <GenericWizard.Page title="Add Marker">
        <Field name="name" validate={Validators.required}>
          {({ input, meta }) => (
            <SemanticForm.Input
              {...input}
              label="Name:"
              error={meta.invalid && meta.touched && meta.error}
              placeholder="Name..."
              required
              onChange={(ev, data) => input.onChange(data.value)}
            />
          )}
        </Field>
        <SemanticForm.Group widths="equal">
          <Field name="latitude" validate={Validators.compose(Validators.required, Validators.number)}>
            {({ input, meta }) => (
              <SemanticForm.Input
                {...input}
                label="Latitude:"
                error={meta.invalid && meta.touched && meta.error}
                placeholder="Latitude..."
                required
                onChange={(ev, data) => input.onChange(data.value)}
              />
            )}
          </Field>
          <Field name="longitude" validate={Validators.compose(Validators.required, Validators.number)}>
            {({ input, meta }) => (
              <SemanticForm.Input
                {...input}
                label="Longitude:"
                error={meta.invalid && meta.touched && meta.error}
                placeholder="Longitude..."
                onChange={(ev, data) => input.onChange(data.value)}
              />
            )}
          </Field>
        </SemanticForm.Group>
      </GenericWizard.Page>
    </GenericWizard>
  );
};

export default AddMarkerWizard;
