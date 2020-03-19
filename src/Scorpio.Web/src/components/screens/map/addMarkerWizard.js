import React from "react";
import { Field } from "react-final-form";
import { Form as SemanticForm } from "semantic-ui-react";
import GenericWizard from "../../common/genericWizard";
import Validators from "../../../utils/formValidators";

const AddMarkerWizard = ({ onClose, onSubmit, initialValues }) => {
  const title = initialValues ? "Edit location" : "Add location";

  return (
    <GenericWizard title={title} onClose={onClose} onSubmit={onSubmit} initialValues={initialValues} showSteps={false}>
      <GenericWizard.Page>
        <Field name="name">
          {({ input, meta }) => (
            <SemanticForm.Input
              {...input}
              label="Name:"
              error={meta.invalid && meta.touched && meta.error}
              placeholder="Name..."
              onChange={(ev, data) => input.onChange(data.value)}
            />
          )}
        </Field>
        <SemanticForm.Group widths="equal">
          <Field
            name="latitude"
            validate={Validators.compose(Validators.required, Validators.number, Validators.minNumber(-90), Validators.maxNumber(90))}
          >
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
          <Field
            name="longitude"
            validate={Validators.compose(Validators.required, Validators.number, Validators.minNumber(-180), Validators.maxNumber(180))}
          >
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
