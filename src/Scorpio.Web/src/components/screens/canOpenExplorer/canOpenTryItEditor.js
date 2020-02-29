import React from "react";
import { Form, Field } from "react-final-form";
import { Form as SemanticForm, Button } from "semantic-ui-react";
import Validators from "../../../utils/formValidators";
import { getDataTypeDropdownOptions, getKeyByValue, dataTypeDict } from "./canOpenDictionaries";
import LogService from "../../../services/LogService";

const CanOpenTryItEditor = ({ index, subIndex, dataType, onTryItSubmit }) => {
  const onSubmit = values => {
    let copy = Object.assign({}, values);
    if (values.dataType) {
      copy = Object.assign(copy, { dataType: getKeyByValue(dataTypeDict, values.dataType) });
    }

    LogService.info("Publishing CAN Open message:", copy);
    if (typeof onTryItSubmit === "function") onTryItSubmit(copy);
  };

  return (
    <Form initialValues={{ index, subIndex, dataType }} onSubmit={onSubmit}>
      {({ handleSubmit, submitting, invalid }) => (
        <>
          <SemanticForm onSubmit={handleSubmit} autoComplete="off" noValidate>
            <SemanticForm.Group widths="equal">
              <Field name="index" validate={Validators.required}>
                {({ input, meta }) => (
                  <SemanticForm.Input
                    {...input}
                    label="Index"
                    error={meta.invalid && meta.touched && meta.error}
                    placeholder="Index..."
                    required
                    readOnly
                    onChange={(ev, data) => input.onChange(data.value)}
                  />
                )}
              </Field>
              <Field name="subIndex" validate={Validators.required}>
                {({ input, meta }) => (
                  <SemanticForm.Input
                    {...input}
                    label="SubIndex"
                    error={meta.invalid && meta.touched && meta.error}
                    placeholder="SubIndex..."
                    required
                    readOnly
                    onChange={(ev, data) => input.onChange(data.value)}
                  />
                )}
              </Field>
              <Field name="dataType" validate={Validators.required}>
                {({ input, meta }) => (
                  <SemanticForm.Select
                    {...input}
                    label="Data type"
                    error={meta.invalid && meta.touched && meta.error}
                    placeholder="Select Data type"
                    required
                    onChange={(_, data) => input.onChange(data.value)}
                    options={getDataTypeDropdownOptions()}
                  />
                )}
              </Field>
              <Field name="value" validate={Validators.required}>
                {({ input, meta }) => (
                  <SemanticForm.Input
                    {...input}
                    label="Value"
                    error={meta.invalid && meta.touched && meta.error}
                    placeholder="Value..."
                    required
                    onChange={(ev, data) => input.onChange(data.value)}
                  />
                )}
              </Field>
            </SemanticForm.Group>
            <Button color="orange" disabled={submitting || invalid}>
              Publish
            </Button>
          </SemanticForm>
        </>
      )}
    </Form>
  );
};

export default CanOpenTryItEditor;
